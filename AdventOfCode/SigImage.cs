using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace AdventOfCode
{
	public class SigImage
	{
		private int[,,] _data;

		public SigImage(int width, int height, int[] rawData)
		{
			Width = width;
			Height = height;
			Layers = rawData.Length / (width * height);

			_data = new int[Layers, height, width];

			for (int l = 0; l < Layers; l++)
			{
				for (int y = 0; y < height; y++)
				{
					for (int x = 0; x < width; x++)
					{
						int i = (l * width * height) + (y * width) + x;

						_data[l, y, x] = rawData[i];
					}
				}
			}
		}

		public int Layers { get; }

		public int Width { get; }

		public int Height { get; }

		public int this[int layer, int y, int x]
		{
			get => _data[layer, y, x];
			set => _data[layer, y, x] = value;
		}

		public int Count(int layer, Func<int, bool> selector)
		{
			int count = 0;
			for (int y = 0; y < Height; y++)
			{
				for (int x = 0; x < Width; x++)
				{
					if (selector(this[layer, y, x]))
						count++;
				}
			}

			return count;
		}

		public SigImage Flatten()
		{
			int[] data = new int[Width * Height];

			for (int y = 0; y < Height; y++)
			{
				for (int x = 0; x < Width; x++)
				{
					int l = 0;
					for (l = 0; l < Layers; l++)
					{
						if (this[l, y, x] != 2)
							break;
					}

					data[(y * Width) + x] = this[l, y, x];
				}
			}

			return new SigImage(Width, Height, data);
		}
	}
}
