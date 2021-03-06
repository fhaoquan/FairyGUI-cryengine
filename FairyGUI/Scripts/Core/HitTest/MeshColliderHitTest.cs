﻿using CryEngine;
using CryEngine.Common;
using CryEngine.EntitySystem;

namespace FairyGUI
{
	/// <summary>
	/// 
	/// </summary>
	public class MeshColliderHitTest : IHitTest
	{
		Entity entity;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="entity"></param>
		public MeshColliderHitTest(Entity entity)
		{
			this.entity = entity;
		}

		public void SetEnabled(bool value)
		{
		}

		public bool HitTest(Container container, ref Vector2 localPoint)
		{
			if (!HitTestContext.raycastDone)
			{
				if (!Global.gEnv.pGameFramework.GetILevelSystem().IsLevelLoaded())
					return false;

				HitTestContext.raycastDone = true;
				Vector3 direction;
#if CE_5_5
				Camera.ScreenPointToDirection((int)HitTestContext.screenPoint.x, (int)HitTestContext.screenPoint.y, out direction);
#else
				direction = Camera.Unproject((int)HitTestContext.screenPoint.x, (int)HitTestContext.screenPoint.y);
#endif
				RaycastHit hit;
				if (Physics.Raycast(Camera.Position, direction, 100, out hit))
				{
					HitTestContext.hitEntityId = hit.EntityId;
					HitTestContext.hitUV = hit.UvPoint;
				}
			}

			if (HitTestContext.hitEntityId != entity.Id)
				return false;

			localPoint = new Vector2(HitTestContext.hitUV.x * container.width, HitTestContext.hitUV.y * container.height);
			HitTestContext.screenPoint = localPoint;

			return true;
		}
	}
}
