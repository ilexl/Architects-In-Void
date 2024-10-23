extends FmodEventEmitter3D

# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	print("AutoSound: playing " + event_name)
	play()
	pass


func _on_stopped() -> void:
	print("AutoSound: stopping " + event_name)
	get_parent().queue_free()
