using System;
using Hmxs.Toolkit.Base.Singleton;
using UnityEngine;
using UnityEngine.Pool;

namespace Net.Scripts.Core
{
	public class ByteBufferPool : SingletonMono<ByteBufferPool>
	{
		[SerializeField] private bool collectionCheck = true;
		[SerializeField] private int maxPoolSize = 500;

		private IObjectPool<ByteBuffer> _pool;

		protected override void Awake()
		{
			base.Awake();

			_pool = new ObjectPool<ByteBuffer>(
				createFunc: () => new ByteBuffer(),
				actionOnRelease: buffer => buffer.Clear(),
				actionOnDestroy: buffer => buffer.Dispose(),
				collectionCheck: collectionCheck,
				maxSize: maxPoolSize
			);
		}

		public ByteBuffer Get() => _pool.Get();

		public void Release(ByteBuffer buffer)
		{
			if (buffer == null) throw new ArgumentNullException(nameof(buffer));
			_pool.Release(buffer);
		}
	}
}
