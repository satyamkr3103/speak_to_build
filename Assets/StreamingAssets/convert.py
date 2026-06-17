import bpy # type: ignore
import sys
import os

argv = sys.argv
argv = argv[argv.index("--") + 1:]

input_file = argv[0]
output_file = argv[1]

bpy.ops.wm.read_factory_settings(
    use_empty=True)

extension = os.path.splitext(
    input_file)[1].lower()

if extension == ".obj":
    bpy.ops.wm.obj_import(
        filepath=input_file)

elif extension == ".fbx":
    bpy.ops.import_scene.fbx(
        filepath=input_file)

elif extension == ".gltf":
    bpy.ops.import_scene.gltf(
        filepath=input_file)

elif extension == ".dae":
    bpy.ops.wm.collada_import(
        filepath=input_file)

elif extension == ".blend":
    bpy.ops.wm.open_mainfile(
        filepath=input_file)

bpy.ops.export_scene.gltf(
    filepath=output_file,
    export_format='GLB')