# Sidims Box - A Implementation between MS-iSTFT-VITS and GPTs, UI made with Unity



# SOON UPDATE WITH USING OTHER METHODS
hold up yall


.
Before actually downloading and try to use this, this project is SERVERLY based on my **OWN COMPUTER**, You would NOT want to run this without any adjustments.
This project isn't even finished yet, so don't use this entire project. It is not recommended.

## Project Structure
![plot](./structure.png)
The project is made in two parts, UI and backend. the UI is fully made in unity, since the original Spine animation is not compatible with python.
The Backend is made with (obviously) python. The backend is made with two parts, LLM and TTS. LLM is just literally GPTs physical API(it isn't out yet) using selenium webdriver.
the TTS is made with MB-iSTFT-VITS multilingual.

When the Backend starts(main.py), The GPTs selenium and TTS would both be initialized, and then when it's fully done, it will print that it's ready to start.
You need to wait until the main.py says it's good to go, since the UI is the client, and it needs python's server to be open.

Backend and UI is connected with TCP socket, using coroutines to run tasks simultaneously. The python part would send unity generated text. 
The UI, when received the text, it will open the generated TTS .wav file via WWW to directly find the file inside of a directory. **THIS IS WHY THIS PROJECT IS NOT FOR DEPLOYMENT.**
The UI would play the sound and show generated text simultaneously.


## Explanation : UI
The UI has three main components. The Avatar(Spine animation), Text Dialouge, Record Button.
The Avatar has it's own script, so it could repond to pat, look at touch point regardless of any state. 
The Text Dialouge just shows sended TCP socket's data.
The Record Button, when pressed would send python "StartRecord" and when clicked again, it would send "StopRecord". This controlls the python to start record for user's voice input, and stops 

**TODO**:  
it should not respond when Arona is actually talking. Also it would be nice to make GPTs to print emotions, making it more *interesting*

