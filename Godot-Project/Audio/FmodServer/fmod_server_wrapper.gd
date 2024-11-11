
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
	if not FmodServer.has_method(currentFunction):
		push_error("FmodServerWrapper: Function not found: " + currentFunction)
	var result = FmodServer.callv(currentFunction, currentArgs)
	currentArgs = []
	return result
