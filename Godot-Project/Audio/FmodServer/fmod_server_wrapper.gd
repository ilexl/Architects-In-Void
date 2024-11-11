
@tool
extends Node

static var currentFunction
static var currentArgs = []

#receive function
static func rf(function):
	print("FMODWrapper: Received new function name")
	currentFunction = function
#receive argument
static func rx(arg):	
	print("FMODWrapper: Received new argument")
	currentArgs.append(arg)
#finalize call
static func fc():
	var printString = "FMODWrapper: Calling function " + currentFunction + " With args: "
	for arg in currentArgs:
		printString += arg + ", "
	print(printString)
	var result = FmodServer.callv(currentFunction, currentArgs)
	currentArgs = []
	return result
