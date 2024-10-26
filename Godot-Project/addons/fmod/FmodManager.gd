@tool
extends Node

var loop_check = 0

func _ready():
	process_mode = PROCESS_MODE_ALWAYS
	var pd = PerformancesDisplay.new()
	if(pd == null):
		_dont_loop_forever()
		_ready()
		return
	add_child(pd)

func _dont_loop_forever() -> void:
	loop_check += 1
	if(loop_check > 1000):
		push_error("infinite loop aborted...")

func _process(delta):
	FmodServer.update()
	
func _notification(what):
	FmodServer.notification(what)
