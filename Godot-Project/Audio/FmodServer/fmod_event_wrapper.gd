@tool
extends Node

static var currentEvent
static var currentFunction
static var currentArgs = []


#receive event
static func re(event):
	currentEvent = event
#receive function
static func rf(function):
	print("FMODEventWrapper: Received new function name")
	currentFunction = function
#receive argument
static func rx(arg):	
	print("FMODEventWrapper: Received new argument")
	currentArgs.append(arg)
#finalize call
static func fc():
	var printString = "FMODEventWrapper: Calling function " + currentFunction + " With args: "
	for arg in currentArgs:
		printString += arg + ", "
	print(printString)
	
	var result = currentEvent.callv(currentFunction, currentArgs)
	
	
	currentArgs = []
	return result
