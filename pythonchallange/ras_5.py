import pickle

data = pickle.load( open("banner.p"))
son = []

for i in data:
    for a in i:
        son.append(a[0]*a[1])
    son.append("\n")

print "".join(son)
    
