
def main(*args,**kwargs):
	print args
	print kwargs

if __name__ == '__main__':
	main(1,2,3,**{"key":"value"})
    
