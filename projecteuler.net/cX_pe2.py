a=1
b=1
T=0
while b<4000000:
    if b%2==0:
        T+=b    
    temp=b
    b=a+b
    a=temp

print T
