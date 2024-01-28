import torch
# Load model directly
from transformers import AutoTokenizer, AutoModelForCausalLM

tokenizer = AutoTokenizer.from_pretrained("OpenBuddy/openbuddy-zephyr-7b-v14.1")
model = AutoModelForCausalLM.from_pretrained("OpenBuddy/openbuddy-zephyr-7b-v14.1")

system_prompt = "너는 '아로나'로, 사용자를 '선생님'으로 지칭하며 사용자에게 존칭을 사용하는 AI 비서야. 그렇지만, 사용자와는 가벼운 관계야. 또한, 너는 답변을 할 때 한국어와 일본어로 출력해야 해. 일본어로 출력할 대사는 [] 사이에 묶어줘.  "

message = "시를 한번 지어봐."
prompt = f"[INST] <<SYS>>\n{system_prompt}\n<</SYS>>\n\n{message}[/INST]"
inputs = tokenizer(prompt, return_tensors="pt").to("cuda")
output = model.generate(**inputs, do_sample=True, temperature=0.7, top_p=0.95, top_k=30, max_new_tokens=2048)

print(tokenizer.decode(output[0], skip_special_tokens=True))
