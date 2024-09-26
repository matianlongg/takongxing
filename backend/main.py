from flask import Flask, request, jsonify
import psycopg2
from psycopg2 import sql

app = Flask(__name__)

# PostgreSQL 数据库连接参数
DATABASE = {
    'dbname': 'run.tbsgame.top',
    'user': 'postgres',
    'password': 'afdzpnDf6Yy2hT8s',
    'host': '101.42.22.222',
    'port': '15432'
}

# 初始化数据库
def init_db():
    conn = psycopg2.connect(**DATABASE)
    cursor = conn.cursor()
    cursor.execute('''
        CREATE TABLE IF NOT EXISTS scores (
            id SERIAL PRIMARY KEY,
            player_name VARCHAR(255) NOT NULL UNIQUE,
            score INT NOT NULL,
            created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
            updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
        )
    ''')
    conn.commit()
    cursor.close()
    conn.close()

@app.route('/submit_score', methods=['POST'])
def submit_score():
    data = request.get_json()
    print(data)
    player_name = data.get('player_name')
    score = data.get('score')

    if player_name is None or score is None:
        return jsonify({'error': 'Missing player_name or score'}), 400

    conn = psycopg2.connect(**DATABASE)
    cursor = conn.cursor()

    # 检查用户是否已提交过得分
    cursor.execute('SELECT score FROM scores WHERE player_name = %s', (player_name,))
    existing_score = cursor.fetchone()

    if existing_score:
        # 更新得分和修改时间
        cursor.execute('''
            UPDATE scores 
            SET score = %s, updated_at = CURRENT_TIMESTAMP 
            WHERE player_name = %s
        ''', (score, player_name))
    else:
        # 插入新得分
        cursor.execute('INSERT INTO scores (player_name, score) VALUES (%s, %s)', (player_name, score))

    conn.commit()
    cursor.close()
    conn.close()

    return jsonify({'message': 'Score submitted/updated successfully!'}), 201

@app.route('/get_scores', methods=['GET'])
def get_scores():
    conn = psycopg2.connect(**DATABASE)
    cursor = conn.cursor()
    cursor.execute('SELECT player_name, score, created_at, updated_at FROM scores ORDER BY score DESC')
    scores = cursor.fetchall()
    cursor.close()
    conn.close()

    return jsonify(scores), 200

if __name__ == '__main__':
    init_db()  # 初始化数据库
    app.run(host='0.0.0.0', port=5000)  # 在本地运行 Flask 应用
