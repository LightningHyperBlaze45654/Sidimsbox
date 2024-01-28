import socket
import time
import os
import pyaudio
import numpy
import wave
import threading
import wave, struct 

from whisper_live.client import TranscriptionClient
client = TranscriptionClient(
  "localhost",
  9090,
  is_multilingual=True,
  lang="ko",
  translate=False,
)
client()