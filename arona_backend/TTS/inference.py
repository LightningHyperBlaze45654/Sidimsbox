import matplotlib.pyplot as plt
import numpy as np
from numpy.fft import fft, ifft
import IPython.display as ipd

import os
import json
import math
import torch
from torch import nn
from torch.nn import functional as F
from torch.utils.data import DataLoader
from playsound import playsound
import re
import TTS.commons as commons
import TTS.utils as utils
from TTS.data_utils import TextAudioLoader, TextAudioCollate, TextAudioSpeakerLoader, TextAudioSpeakerCollate
from TTS.models import SynthesizerTrn
from TTS.text.symbols import symbols
from TTS.text import text_to_sequence

from scipy.io.wavfile import write
import pyaudio
import TTS.testreader as testreader



def get_text(text, hps):
    text_norm = text_to_sequence(text, hps.data.text_cleaners)
    if hps.data.add_blank:
        text_norm = commons.intersperse(text_norm, 0)
    text_norm = torch.LongTensor(text_norm)
    return text_norm

hps = utils.get_hparams_from_file("./TTS/configs/arona.json")

net_g = SynthesizerTrn(
    len(symbols),
    hps.data.filter_length // 2 + 1,
    hps.train.segment_size // hps.data.hop_length,
    **hps.model).cuda()
_ = net_g.eval()

_ = utils.load_checkpoint("./TTS/models/arona/G_129000.pth", net_g, None)

def generate_audio(response):
    stn_tst = get_text(response, hps)
    
    with torch.no_grad():
        x_tst = stn_tst.cuda().unsqueeze(0)
        x_tst_lengths = torch.LongTensor([stn_tst.size(0)]).cuda()
        audio = net_g.infer(x_tst, x_tst_lengths, noise_scale=.667, noise_scale_w=0.8, length_scale=1)[0][0,0].data.cpu().float().numpy()
    out_path="./output.wav"
    write(out_path, hps.data.sampling_rate, audio)


