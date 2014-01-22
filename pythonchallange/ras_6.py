import zipfile
import re

file = zipfile.ZipFile("channel.zip", "r")
data = {}

for name in file.namelist():
    data[name] = file.read(name)

nothing = 90052
comment=[]
while True:
    a = str(nothing)+".txt"
    ara = re.search("\d+\d$", data[a])
    if ara:
        nothing = ara.group()
        comment.append(file.getinfo(a).comment)
        print nothing
    else:
        print data[a]
        break

print "".join(comment)
