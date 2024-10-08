using System.Collections.Generic;
using System.Runtime.InteropServices;
using ArchitectsInVoid.Debug.Meshes;
using Godot;

namespace ArchitectsInVoid.Debug;

public sealed partial class DebugDraw : Node
{


    
    public static SceneTree SceneTree = (SceneTree)Engine.GetMainLoop();
    public static Window Root = SceneTree.GetRoot();
    
    
    DebugDraw()
    {
        Root.AddChild(this);
    }

    #region Singleton
    private static DebugDraw _instance = null;
    private static readonly object Padlock = new object();

    private static DebugDraw Instance
    {
        get
        {
            lock (Padlock)
            {
                if (_instance == null)
                {
                    _instance = new DebugDraw();
                }

                return _instance;
            }
        }
    }
    #endregion
    #region Instanced

    #region TODO: This shouldn't be here
    private bool _cameraNormalThisFrame = false;
    private Vector3 _cameraNormal;

    internal Vector3 CameraNormal
    {
        get
        {
            if (!_cameraNormalThisFrame)
            {
                _cameraNormal = Root.GetCamera3D().GlobalTransform.Basis.Z;
                _cameraNormalThisFrame = true;
            }
            return _cameraNormal;
        }
    }

    private bool _cameraPositionThisFrame = false;
    private Vector3 _cameraPosition;

    internal Vector3 CameraPosition
    {
        get
        {
            if (!_cameraPositionThisFrame)
            {
                _cameraPosition = Root.GetCamera3D().GlobalPosition;
                _cameraPositionThisFrame = true;
            }
            return _cameraPosition;
        }
    }

    private bool _cameraRotationThisFrame = false;
    private Vector3 _cameraRotation;
    public Vector3 CameraWorldRotation
    {
        get
        {
            if (!_cameraRotationThisFrame)
            {
                _cameraRotation = Root.GetCamera3D(). GlobalTransform.Basis.GetEuler();;
                _cameraRotationThisFrame = true;
            }
           
            return _cameraRotation;
        }
    }
#endregion

    private double _lastDelta = 0;
    private List<Meshes.DebugMesh> _debugObjects = new();
    public override void _Process(double delta)
    {
        _lastDelta = delta;
        _cameraNormalThisFrame = false;
        _cameraPositionThisFrame = false;
        _cameraRotationThisFrame = false;
        for (var index = _debugObjects.Count - 1; index >= 0; index--)
        {
            var mesh = _debugObjects[index];
            bool shouldRemove = mesh.Update(delta);
            if (shouldRemove)
            {
                mesh.QueueFree();
                _debugObjects.RemoveAt(index);
            }
        }

        // Cheaper to reuse both lists than to instantiate a new one
    }
    #endregion

    private static void InstantiateDebugMesh(Meshes.DebugMesh mesh)
    {
        Root.AddChild(mesh);
        Instance._debugObjects.Add(mesh);
    }
        
    #region DebugDefaults
            
    private const double DefaultDuration = 1.0 / 60; // TODO: Replace this with update rate from project settings
    private const double DefaultThickness = 0;
    private const double DefaultRadius = 1;
    private const int DefaultPrecision = 20;
    
    private static readonly Color DefaultColor = new Color(255f, 255f, 255f);
    private static readonly Vector3 DefaultPos = Vector3.Zero;
    #endregion
        
    #region DebugLine
    /// <summary>
    /// Draws a line between two points with optional arguments.
    /// </summary>
    /// <param name="start">Start position of line. Defaults to Vector3.Zero.</param>
    /// <param name="end">End position of line. Defaults to Vector3.Zero.</param>
    /// <param name="color">Color of line. Defaults to White.</param>
    /// <param name="duration">Time until the line gets destroyed. Defaults to 1 frame.</param>
    /// <param name="thickness">Apparent "radius" of the line. Defaults to 0.</param>
    /// <param name="drawOnTop">Controls whether the object draws on top of everything else.</param>
    /// <param name="type">Auto, Tangible, or Wireframe. Tangible produces a line with a radius, wireframe produces an infinitely small yet visible line. Auto is Wireframe if 0, otherwise tangible. Defaults to Auto.</param>
    public static void Line([Optional]Vector3? start, [Optional]Vector3? end, [Optional]Color? color, double duration = DefaultDuration, double thickness = DefaultThickness, bool drawOnTop = false, DebugMesh.Type type = DebugMesh.Type.Auto )
    {
        Vector3 a = start ?? DefaultPos;
        Vector3 b = end ?? DefaultPos;
        Color finalColor = color ?? DefaultColor;



        DebugLine mesh = new DebugLine(a, b, finalColor, duration + Instance._lastDelta / 2, thickness, drawOnTop, type, Instance);
        

        InstantiateDebugMesh(mesh);
        
    }
    
    #endregion
    #region DebugCircle
        
        /// <summary>
        /// Draws a circle at a position with optional arguments.
        /// </summary>
        /// <param name="position">The position to draw the circle. Defaults to Vector3.Zero.</param>
        /// <param name="color">The color of the circle. Defaults to white.</param>
        /// <param name="duration">The time after which the circle will be destroyed. Defaults to one frame.</param>
        /// <param name="radius">The radius of the circle. Defaults to 1.</param>
        /// <param name="precision">The amount of points to construct the circle with. Defaults to 20.</param>
        /// <param name="drawOnTop">Controls whether the object draws on top of everything else.</param>
        /// <param name="type">The type of the circle, being solid or wireframe. Defaults to solid.</param>
    public static void Circle([Optional]Vector3? position, [Optional]Color? color, double duration = DefaultDuration, double radius = DefaultRadius, int precision = DefaultPrecision, bool drawOnTop = false, DebugMesh.Type type = DebugMesh.Type.Auto )
    {
        Vector3 pos = position ?? DefaultPos;
        Color finalColor = color ?? DefaultColor;
    
    
    
        DebugCircle mesh = new DebugCircle(pos, finalColor, duration, radius, precision, drawOnTop, type, Instance);
        
    
        InstantiateDebugMesh(mesh);
        
    }
    #endregion
    #region DebugRaycast
    public static void Ray(PhysicsRayQueryParameters3D query, Godot.Collections.Dictionary result, double duration = DefaultDuration)
    {
        Vector3 start = query.From;
        Vector3 end = query.To;
        

        if (result.Count > 0)
        {
            Vector3 hit = (Vector3)result["position"];
            Vector3 normal = (Vector3)result["normal"];
            Line(start, hit, Colors.Red, duration, drawOnTop:true);
            Line(hit, end, Colors.Green, duration, drawOnTop:true);
            Line(hit, hit + normal, Colors.Aqua, duration, drawOnTop:true);
            return;
        }
        Line(start, end, Colors.Red, duration, drawOnTop:true);
    }    
        
    #endregion
        // TODO: implement
    #region DebugSphere
    #endregion
        // TODO: Implement
    #region DebugLabel
    #endregion
}