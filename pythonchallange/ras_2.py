import string

metin = """g fmnc wms bgblr rpylqjyrc gr zw fylb. rfyrq ufyr amknsrcpq ypc dmp. bmgle gr gl zw fylb gq glcddgagclr ylb rfyr'q ufw rfgq rcvr gq qm jmle. sqgle qrpgle.kyicrpylq() gq pcamkkclbcb. lmu ynnjw ml rfc spj."""
kaynak= "yzabcdefghijklmnopqrstuvwx"
hedef = "abcdefghijklmnopqrstuvwxyz"

cevir = string.maketrans(kaynak,hedef)

soncevir = metin.translate(cevir)

print soncevir
