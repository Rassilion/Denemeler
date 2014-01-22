import re
import urllib2

nothing = 12345
while True:
    url = urllib2.urlopen("http://www.pythonchallenge.com/pc/def/linkedlist.php?nothing=%s" %nothing)
    sayfa = url.read()
    ara = re.search("\d+\d",sayfa)
    if ara:
        nothing = ara.group()
        print nothing
    else:
        print nothing
        break
