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
	currentFunction = function
#receive argument
static func rx(arg):	
	currentArgs.append(arg)
#finalize call
static func fc():
	var result = currentEvent.callv(currentFunction, currentArgs)
	currentArgs = []
	return result

#get property
static func gp(property_name : String):
	return currentEvent[property_name]
	
#set property
static func sp(property_name : String, value):
	currentEvent[property_name] = value
