extends Node3D


# Called when the node enters the scene tree for the first time.
func _ready():
	var cam = get_node("Player/Plane/springarm/camera")
	if cam == null:
		push_error("Camera not found!")
		return
	cam.make_current()


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	pass
