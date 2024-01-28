import pandas as pd
import selenium
from selenium import webdriver
from selenium.webdriver.common.by import By
from selenium.webdriver.common.keys import Keys
from time import sleep
import undetected_chromedriver as uc
from selenium.webdriver.support.ui import WebDriverWait
from fake_useragent import UserAgent
from selenium.webdriver.support import expected_conditions as EC
import LLM.helper_funcs as helper_funcs



# print(prompt)

op = uc.ChromeOptions()
op.add_argument(f"user-agent={UserAgent.random}")
op.add_argument("window-size=720,720")


driver = uc.Chrome(chrome_options=op, enable_cdp_events=True)
helper_fn = helper_funcs.HelperFn(driver)

MAIL = "lightninghyperblaze456@gmail.com"
PASSWORD = "GLove1052!!"


driver.get('https://chat.openai.com/c/40f865f5-b4d8-43b6-95bf-708081531aac')
sleep(1)
inputElements = driver.find_elements(By.TAG_NAME, "button")
inputElements[0].click()
sleep(1)
google_btn = driver.find_elements(By.CSS_SELECTOR,'button[data-provider="google"]')[0]
google_btn.click()

mail_xpath = "//*[@type='email']"
helper_fn.wait_for_element_visible(mail_xpath)
mail = helper_fn.find_element(mail_xpath)
sleep(0.5)
mail.send_keys(MAIL)
mail.send_keys(Keys.ENTER)

password_xpath = "//*[@type='password']"
helper_fn.wait_for_element_visible(password_xpath)
sleep(0.5)
password = helper_fn.find_element(password_xpath)
password.send_keys(PASSWORD)
password.send_keys(Keys.ENTER)

def send_and_recieve(prompt):
    inputbox_path = '//*[@id="prompt-textarea"]'
    helper_fn.wait_for_element_visible(inputbox_path)
    inputbox = helper_fn.find_element(inputbox_path)
    inputbox.send_keys(prompt)
    inputbox.send_keys(Keys.ENTER)

    sendicon_path = '//*[@data-testid="send-button"]'
    helper_fn.wait_for_element_visible(sendicon_path)
    result_div = driver.find_elements(By.XPATH, '//*[@class="markdown prose w-full break-words dark:prose-invert light"]')[-1]

    return result_div.text