extends Node2D

signal generated

#########################
# Internal
# Onready
onready var _sprite = $Renderer
onready var _textureRect = $Viewport/Shader
onready var _viewport = $Viewport

export var _viewer: NodePath

# ###
var _genImage
export var _texture: Texture

func get_image() -> Image:
	if _genImage != null:
		return _genImage
	else:
		printerr("No image generated, use generate_image() and wait for \"generated\" signal")
		return null

func _ready():
	_textureRect.texture = _texture


func _on_load_image():
	_texture = get_node(_viewer).texture
	_textureRect.texture = _texture


func generate_image(material : Material):
	# Resize generating nodes
	var resolution = _texture.get_size()
	_viewport.size = resolution
	_viewport.render_target_update_mode = Viewport.UPDATE_ALWAYS
	_textureRect.rect_size = resolution
	
	# Set material type
	_textureRect.set_material(material)

	## Actually Generate Image
	_sprite.show()
	yield(get_tree(),"idle_frame")
	yield(get_tree(),"idle_frame")
	yield(get_tree(),"idle_frame")
	_genImage = _sprite.get_texture().get_data().duplicate()
	emit_signal("generated")
	_viewport.render_target_update_mode = Viewport.UPDATE_DISABLED
	_sprite.hide()
