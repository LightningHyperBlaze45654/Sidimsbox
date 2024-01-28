import socket
import os
import TTS.inference as aronaTTS

import whisper
import pyaudio
import numpy as np
import wave
import threading
import time

import LLM.chatgpt_selenium as gpts

#wake_word = "아로나"
tiny_model_path = "C:/Users/user/.cache/whisper/tiny.pt"
small_model_path = os.path.expanduser('C:/Users/user/.cache/whisper/small.pt')
print("Whisper model loading")
#tiny_model = whisper.load_model(tiny_model_path)
small_model = whisper.load_model(small_model_path)
print("Whisper model loaded")

#Uses inference.py to generate tts via text
def generate_tts(text):
    aronaTTS.generate_audio(text)

# Global flag to indicate whether to listen or not
listening_flag = False


def listen_audioinput():
    global listening_flag
    FORMAT = pyaudio.paInt16
    CHANNELS = 1
    RATE = 44100
    CHUNK = 1024

    p = pyaudio.PyAudio()                                                                                                                                                  

    stream = p.open(format=FORMAT,
                    channels=CHANNELS,
                    rate=RATE,
                    input=True,
                    frames_per_buffer=CHUNK)

    audio_data = []

    while listening_flag:
        data = stream.read(CHUNK)
        audio_data.append(data)

    # Stop the stream and close PyAudio when listening is stopped
    stream.stop_stream()
    stream.close()
    p.terminate()
    
    # Write recorded audio to a WAV file
    with wave.open("./prompt.wav", 'wb') as wf:
        wf.setnchannels(CHANNELS)
        wf.setsampwidth(p.get_sample_size(FORMAT))
        wf.setframerate(RATE)
        wf.writeframes(b''.join(audio_data))


def handle_client(client_socket):
    global listening_flag
    while True:
        data = client_socket.recv(1024).decode("UTF-8")
        if not data:
            break
        print(f"Received data: {data}")
        if data == "StartRecord" and not listening_flag:
            listening_flag = True
            # Start a new thread for listening
            listen_thread = threading.Thread(target=listen_audioinput)
            listen_thread.start()

        elif data == "StopRecord" and listening_flag:
            listening_flag = False
            # You may want to wait for the listen_thread to finish before proceeding
            listen_thread.join()
            print(gpts.send_and_recieve(small_model.transcribe("./prompt.wav")))
            
        # Process the received data, e.g., start/stop recording

    print("Closing connection")
    client_socket.close()

def send_server_data(client_socket, data):
    # Send the provided data to the client
    client_socket.send(data.encode("UTF-8"))

def start_server():
    server_ip = "127.0.0.1"
    server_port = 25001

    server = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
    server.bind((server_ip, server_port))
    server.listen()

    print(f"Server listening on {server_ip}:{server_port}")

    while True:
        client_socket, client_address = server.accept()
        print(f"Connection from {client_address}")

        # Start a new thread to handle the client
        client_thread = threading.Thread(target=handle_client, args=(client_socket,))
        client_thread.start()

        # Send strings to the client using send_server_data
        # Wait for a while (adjust as needed) and then close the connection

if __name__ == "__main__":
    start_server()

