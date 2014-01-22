import zipfile
import re

file = zipfile.ZipFile("channel.zip", "r")
data = {}

for name in file.namelist():
    data[name] = file.read(name)

nothing = 100
while True:
    a = str(nothing)+".txt"
    ara = re.search("\d+\d$", data[a])
    if ara:
        nothing = ara.group()
        print nothing
    else:
        print data[a]
        break
