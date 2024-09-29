import base64
import json
from flask import Flask, request, jsonify
import psycopg2
from psycopg2 import sql
import base64
from Crypto.Cipher import AES

app = Flask(__name__)

ENCRYPTION_KEY = b'urDw32mgb9DZ5Fv3V5AvhwcM'  # 必须是16, 24 或 32 bytes

def decrypt_data(encrypted_data):
    encrypted_data_bytes = base64.b64decode(encrypted_data)

    # 提取IV和加密的正文
    iv = encrypted_data_bytes[:16]
    encrypted_message = encrypted_data_bytes[16:]

    cipher = AES.new(ENCRYPTION_KEY, AES.MODE_CBC, iv)
    decrypted_message = cipher.decrypt(encrypted_message)

    # 去除填充
    unpadded_message = decrypted_message[:-decrypted_message[-1]]
    
    return unpadded_message.decode('utf-8')


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
    encrypted_data = request.get_data(as_text=True)

    try:
        # 解密数据
        decrypted_data = decrypt_data(encrypted_data)
        data = json.loads(decrypted_data)
    except Exception as e:
        return jsonify({'error': 'Invalid encrypted data or decryption failed'}), 400
    
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
        # 将结果转换为字典列表
    score_list = []
    for score in scores:
        score_list.append({
            'player_name': score[0],
            'score': score[1],
            'created_at': score[2],
            'updated_at': score[3]
        })

    return jsonify(score_list), 200


if __name__ == '__main__':
    init_db()  # 初始化数据库
    app.run(host='0.0.0.0', port=5000)  # 在本地运行 Flask 应用
