# -*- coding: utf-8 -*-

from __future__ import division

while True:
	print "1 toplama \n 2 çıkarma \n 3 çarpma \n 4 bölme"
	
	i1 = raw_input("işlem numarası")
	if i1 == "1":
		a = float(raw_input("ilk sayı"))
		print a
		b = float(raw_input("ikinci sayı"))
		print a, "+", b, "=", a + b
	if i1 == "2":
		a = float(raw_input("ilk sayı"))
		print a
		b = float(raw_input("ikinci sayı"))
		print a, "-", b, "=", a - b
	if i1 == "3":
		a = float(raw_input("ilk sayı"))
		print a
		b = float(raw_input("ikinci sayı"))
		print a, "*", b, "=", a * b
	if i1 == "4":
		a = float(raw_input("ilk sayı"))
		print a
		b = float(raw_input("ikinci sayı"))
		print a, "/", b, "=", a / b
	else:
		print "yanlış işlem numarası"