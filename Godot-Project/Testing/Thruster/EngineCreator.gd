extends Node3D

@export var Width:float = 2.5
@export var Height:float = 2.5
@export var Length:float = 10


@export var NozzleLength:float = 1.5
@export var EngineCornerRadius:float = 0.5

var surfaceArea:float
var volume:float

var thrust:float # newtons	
var materialUse:float # material used per second
var efficiency:float # material used per unit of thrust
var thrustSpoolTime:float # time taken to go from 0 - 100% thrust
var thrustPerVolume:float

# Called when the node enters the scene tree for the first time.
func _ready():
	surfaceArea = Width * Height
	volume = surfaceArea * Length
	var lengthSquared:float = Length * Length
	
	# resources per unit of thrust - calculated such that length has a large weight in efficiency
	efficiency = lengthSquared / surfaceArea
	# calculate spool time as the inverse of efficiency, to be changed
	thrustSpoolTime = surfaceArea / lengthSquared
	# multiply width + hight by length * length to favor length in thrust calculation
	thrust = (Width + Height) * lengthSquared
	# and then we simply calculate material use as tbrust by efficiency
	materialUse = thrust * efficiency
	# statistical number
	thrustPerVolume = thrust/volume
	print("Surface area: " + str(surfaceArea) + " m^2")
	print("Volume: " + str(volume) + " m^3")
	print("Thrust per second: " + str(thrust))
	print("Thrust per volume: " + str(thrustPerVolume))
	print("Resource per second: " + str(materialUse))
	print("Thrust per resource: " + str(thrust / materialUse))
	
	print ("Full thrust time: " + str(thrustSpoolTime))
#	print("Surface area is " + surfaceArea + " m^2")
#	print("Volume is " + surfaceArea 
	add_rounded_box(Vector3(Width - 0.1, Height - 0.1, Length - NozzleLength), Vector3(0, 0, -NozzleLength / 2), EngineCornerRadius)

	
	generate_fins()
	
@export var FinSpacing:float = 0.2
@export var FinWidth:float = 0.05
func generate_fins():
	add_box(Vector3(Width, 0.1, NozzleLength), Vector3(0, Height / 2, (Length - NozzleLength) / 2 ))
	add_box(Vector3(Width, 0.1, NozzleLength), Vector3(0, -Height / 2, (Length - NozzleLength) / 2 ))
	add_box(Vector3(0.1, Height, NozzleLength), Vector3(Width / 2, 0, (Length - NozzleLength) / 2 ))
	add_box(Vector3(0.1, Height, NozzleLength), Vector3(-Width / 2, 0, (Length - NozzleLength) / 2 ))
	
	var startPos:float = -Height / 2
	var finCount:int = Height / FinSpacing
	for i in finCount:
		var pos:float = startPos + FinSpacing * i + FinSpacing
		add_box(Vector3(Width, FinWidth, NozzleLength), Vector3(0, pos, (Length - NozzleLength) / 2))
		
		
	
func add_box(s:Vector3, p:Vector3):
	var boxMesh = MeshInstance3D.new()
	boxMesh.mesh = BoxMesh.new()
	boxMesh.scale = s
	boxMesh.position = p
	add_child(boxMesh)
func add_cylinder(s:Vector3, p:Vector3, r:Vector3):
	var cylinderMesh = MeshInstance3D.new()
	cylinderMesh.mesh = CylinderMesh.new()
	cylinderMesh.scale = s
	cylinderMesh.position = p
	cylinderMesh.rotation = r
	add_child(cylinderMesh)
func add_sphere(s:float, p:Vector3): # assumes a perfect sphere
	var sphereMesh = MeshInstance3D.new()
	sphereMesh.mesh = SphereMesh.new()
	sphereMesh.scale = Vector3(s, s, s)
	sphereMesh.position = p
	add_child(sphereMesh)
func add_rounded_box(s:Vector3, p:Vector3, r:float):
	
	# Where S is scale, P is position, R is radius
	var d = r * 2
	add_box(Vector3(s.x, s.y - d, s.z - d), p)
	add_box(Vector3(s.x - d, s.y, s.z - d), p)
	add_box(Vector3(s.x - d, s.y - d, s.z), p)
	
	var cylinderScaleA = Vector3(d, s.y / 2 - r, d)
	var cylinderRotA = Vector3(0, 0, 0)
	var cylinderPosAA = Vector3(p.x + s.x / 2 - r, p.y, p.z + s.z / 2 - r)
	var cylinderPosAB = Vector3(p.x + s.x / 2 - r, p.y, p.z - s.z / 2 + r)
	var cylinderPosAC = Vector3(p.x - s.x / 2 + r, p.y, p.z - s.z / 2 + r)
	var cylinderPosAD = Vector3(p.x - s.x / 2 + r, p.y, p.z + s.z / 2 - r)
	add_cylinder(cylinderScaleA, cylinderPosAA, cylinderRotA)	
	add_cylinder(cylinderScaleA, cylinderPosAB, cylinderRotA)	
	add_cylinder(cylinderScaleA, cylinderPosAC, cylinderRotA)	
	add_cylinder(cylinderScaleA, cylinderPosAD, cylinderRotA)	
	
	var cylinderScaleB = Vector3(d, s.x / 2 - r, d)
	var cylinderRotB = Vector3(0, 0, PI/2)
	var cylinderPosBA = Vector3(p.x, p.y + s.y / 2 - r, p.z + s.z / 2 - r)
	var cylinderPosBB = Vector3(p.x, p.y + s.y / 2 - r, p.z - s.z / 2 + r)
	var cylinderPosBC = Vector3(p.x, p.y - s.y / 2 + r, p.z - s.z / 2 + r)
	var cylinderPosBD = Vector3(p.x, p.y - s.y / 2 + r, p.z + s.z / 2 - r)
	add_cylinder(cylinderScaleB, cylinderPosBA, cylinderRotB)
	add_cylinder(cylinderScaleB, cylinderPosBB, cylinderRotB)
	add_cylinder(cylinderScaleB, cylinderPosBC, cylinderRotB)
	add_cylinder(cylinderScaleB, cylinderPosBD, cylinderRotB)
	
	var cylinderScaleC = Vector3(d, s.z / 2 - r, d)
	var cylinderRotC = Vector3(PI/2, 0, 0)
	var cylinderPosCA = Vector3(p.x + s.x / 2 - r, p.y + s.y / 2 - r, p.z)
	var cylinderPosCB = Vector3(p.x + s.x / 2 - r, p.y - s.y / 2 + r, p.z)
	var cylinderPosCC = Vector3(p.x - s.x / 2 + r, p.y - s.y / 2 + r, p.z)
	var cylinderPosCD = Vector3(p.x - s.x / 2 + r, p.y + s.y / 2 - r, p.z)
	
	add_cylinder(cylinderScaleC, cylinderPosCA, cylinderRotC)
	add_cylinder(cylinderScaleC, cylinderPosCB, cylinderRotC)
	add_cylinder(cylinderScaleC, cylinderPosCC, cylinderRotC)
	add_cylinder(cylinderScaleC, cylinderPosCD, cylinderRotC)
	
	var hs = Vector3(s.x / 2 - r, s.y / 2 - r, s.z / 2 - r)
	var ca = p + Vector3(hs.x, hs.y, hs.z)
	var cb = p + Vector3(-hs.x, hs.y, hs.z)
	var cc = p + Vector3(hs.x, -hs.y, hs.z)
	var cd = p + Vector3(-hs.x, -hs.y, hs.z)
	var ce = p + Vector3(hs.x, hs.y, -hs.z)
	var cf = p + Vector3(-hs.x, hs.y, -hs.z)
	var cg = p + Vector3(hs.x, -hs.y, -hs.z)
	var ch = p + Vector3(-hs.x, -hs.y, -hs.z)	
	
	
	add_sphere(d, ca)
	add_sphere(d, cb)
	add_sphere(d, cc)
	add_sphere(d, cd)
	add_sphere(d, ce)
	add_sphere(d, cf)
	add_sphere(d, cg)
	add_sphere(d, ch)
	
	
	
	
	
# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	pass
