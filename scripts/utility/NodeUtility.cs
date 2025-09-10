using Godot;

public class NodeUtility
{
    /// <summary>
    /// Traverses up the node hierarchy starting from the specified <paramref name="startNode"/> and returns the first group name
    /// from <paramref name="sceneGroupNames"/> that a parent node belongs to.
    /// </summary>
    /// <param name="startNode">The node from which to start searching up the hierarchy.</param>
    /// <param name="sceneGroupNames">A list of group names to check for membership in each parent node.</param>
    /// <returns>
    /// The name of the first group found that a parent node belongs to; otherwise, <c>null</c> if no parent node is in any of the specified groups.
    /// </returns>
    public static string GetParentNodeGroup(Node startNode, params string[] sceneGroupNames)
    {
        Node parent = startNode;
        while (parent != null)
        {
            foreach (string groupName in sceneGroupNames)
            {
                if (parent.IsInGroup(groupName))
                {
                    return groupName;
                }
            }
            parent = parent.GetParent();
        }
        return null;
    }
}