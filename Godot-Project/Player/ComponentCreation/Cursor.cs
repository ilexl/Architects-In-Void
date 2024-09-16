using Godot;

using System.Collections.Generic;

namespace ArchitectsInVoid.Player.ComponentCreation;

public partial class Cursor : Node3D
{
	private const int EdgeCount = 4;
	
	
	private Dictionary<char, List<MeshInstance3D>> _edges;
	
	private Label3D _label;
	public override void _Ready()
	{
		_edges = new Dictionary<char, List<MeshInstance3D>>()
		{
			{ 'X', new List<MeshInstance3D>(EdgeCount) },
			{ 'Y', new List<MeshInstance3D>(EdgeCount) },
			{ 'Z', new List<MeshInstance3D>(EdgeCount) }
		};
		foreach (var edgeGroup in _edges)
		{
			for (int i = 0; i < EdgeCount; i++)
			{
				var meshInstance = new MeshInstance3D();
				meshInstance.Mesh = new BoxMesh();
				meshInstance.Scale = new Vector3(0.1f, 0.1f, 0.1f);
				AddChild(meshInstance);
				edgeGroup.Value.Add(meshInstance);
			}
		}
		_label = GetNode<Label3D>("Object Text");
	}

	
	public override void _Process(double delta)
	{
	}
	
	new public void SetScale(Vector3 scale)
	{
		
		var xEdges = _edges['X'];
		var yEdges = _edges['Y'];
		var zEdges = _edges['Z'];

		ScaleEdgeSet(xEdges, new Vector3(scale.X, 0.1, 0.1), new Vector3(0, scale.Y, scale.Z));
		ScaleEdgeSet(yEdges, new Vector3(0.1, scale.Y, 0.1), new Vector3(scale.X, 0, scale.Z));
		ScaleEdgeSet(zEdges, new Vector3(0.1, 0.1, scale.Z), new Vector3(scale.X, scale.Y, 0));
	}

	private void ScaleEdgeSet(List<MeshInstance3D> edges, Vector3 scale, Vector3 pos)
	{
		for (int i = 0; i < 4; i++)
		{
			edges[i].Scale = scale;
		}

		Vector3 max = pos / 2;
		Vector3 min = -max;

		edges[0].Position = max;
		edges[1].Position = new Vector3(min.X, max.Y, min.Z);
		edges[2].Position = new Vector3(max.X, min.Y, min.Z);
		edges[3].Position = new Vector3(min.X, min.Y, max.Z);
	}

	private void SetLabelName(PackedScene scene)
	{
		if (scene == null)
		{
			_label.Text = "No block selected!";
		}

		_label.Text = scene.ResourcePath;

	}
}