


using Alex.ResourcePackLib.Json.Models.Entities;
using Microsoft.Xna.Framework;

namespace Alex.Entities.Models 
{

	public partial class SlimeModel : EntityModel
	{
		public SlimeModel()
		{
			Name = "geometry.slime";
			VisibleBoundsWidth = 5;
			VisibleBoundsHeight = 2;
			VisibleBoundsOffset = new Vector3(0f, 1f, 0f);
			Texturewidth = 64;
			Textureheight = 32;
			Bones = new EntityModelBone[4]
			{
				new EntityModelBone(){ 
					Name = "cube",
					Parent = "",
					Pivot = new Vector3(0f,24f,0f),
					Rotation = new Vector3(0f,0f,0f),
					NeverRender = false,
					Mirror = false,
					Reset = false,
					Cubes = new EntityModelCube[1]{
						new EntityModelCube()
						{
							Origin = new Vector3(-3f,1f,-3f),
							Size = new Vector3(6f, 6f, 6f),
							Uv = new Vector2(0f, 16f)
						},
					}
				},
				new EntityModelBone(){ 
					Name = "eye0",
					Parent = "cube",
					Pivot = new Vector3(0f,24f,0f),
					Rotation = new Vector3(0f,0f,0f),
					NeverRender = false,
					Mirror = false,
					Reset = false,
					Cubes = new EntityModelCube[1]{
						new EntityModelCube()
						{
							Origin = new Vector3(-3.3f,4f,-3.5f),
							Size = new Vector3(2f, 2f, 2f),
							Uv = new Vector2(32f, 0f)
						},
					}
				},
				new EntityModelBone(){ 
					Name = "eye1",
					Parent = "cube",
					Pivot = new Vector3(0f,24f,0f),
					Rotation = new Vector3(0f,0f,0f),
					NeverRender = false,
					Mirror = false,
					Reset = false,
					Cubes = new EntityModelCube[1]{
						new EntityModelCube()
						{
							Origin = new Vector3(1.3f,4f,-3.5f),
							Size = new Vector3(2f, 2f, 2f),
							Uv = new Vector2(32f, 4f)
						},
					}
				},
				new EntityModelBone(){ 
					Name = "mouth",
					Parent = "cube",
					Pivot = new Vector3(0f,24f,0f),
					Rotation = new Vector3(0f,0f,0f),
					NeverRender = false,
					Mirror = false,
					Reset = false,
					Cubes = new EntityModelCube[1]{
						new EntityModelCube()
						{
							Origin = new Vector3(0f,2f,-3.5f),
							Size = new Vector3(1f, 1f, 1f),
							Uv = new Vector2(32f, 8f)
						},
					}
				},
			};
		}

	}

}