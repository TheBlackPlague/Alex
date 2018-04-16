﻿using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Alex.API.Gui;
using Alex.API.Gui.Elements;
using Alex.API.Gui.Elements.Controls;
using Alex.API.Gui.Rendering;
using Alex.API.Utils;
using Alex.Gamestates.Gui;
using Alex.GameStates.Gui.MainMenu;
using Alex.Graphics;
using Alex.Graphics.Models;
using Alex.Utils;
using Alex.Worlds;
using Alex.Worlds.Generators;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Alex.Gamestates
{
	public class TitleState : GameState
	{
		private GuiDebugInfo _debugInfo;

		private GuiTextElement _splashText;

		private GuiPanoramaSkyBox _backgroundSkyBox;

		private FpsMonitor FpsMonitor { get; }
		public TitleState(Alex alex, ContentManager content) : base(alex)
		{
			FpsMonitor = new FpsMonitor();
			_backgroundSkyBox = new GuiPanoramaSkyBox(alex, alex.GraphicsDevice, content);

			Gui = new GuiScreen(Alex)
			{
				//DefaultBackgroundTexture = GuiTextures.OptionsBackground
			};
			var stackMenu = new GuiStackMenu()
			{
				LayoutOffsetX = 25,
				Width = 125,
				VerticalAlignment = VerticalAlignment.Center,

				VerticalContentAlignment = VerticalAlignment.Top,
				HorizontalContentAlignment = HorizontalAlignment.Stretch
			};

			stackMenu.AddMenuItem("Multiplayer", () =>
			{
				//TODO: Switch to multiplayer serverlist (maybe choose PE or Java?)
				Alex.ConnectToServer();
			});

			stackMenu.AddMenuItem("Multiplayer Servers", () =>
			{
				Alex.GameStateManager.SetActiveState<MultiplayerServerSelectionState>();
			});

			stackMenu.AddMenuItem("Debug Blockstates", DebugWorldButtonActivated);
			stackMenu.AddMenuItem("Debug Flatland", DebugFlatland);
			stackMenu.AddMenuItem("Debug Anvil", DebugAnvil);

			stackMenu.AddMenuItem("Options", () => { Alex.GameStateManager.SetActiveState("options"); });
			stackMenu.AddMenuItem("Exit Game", () => { Alex.Exit(); });

			Gui.AddChild(stackMenu);

			Gui.AddChild(new GuiImage(GuiTextures.AlexLogo)
			{
				//LayoutOffsetX = 175,
				LayoutOffsetY = 25,
				HorizontalAlignment = HorizontalAlignment.Center,
				VerticalAlignment = VerticalAlignment.Top
			});
			Gui.AddChild( _splashText = new GuiTextElement(false)
			{
				TextColor = TextColor.Yellow,
				Rotation = 17.5f,
				//RotationOrigin = Vector2.Zero,
				
				X = 240,
				Y = 15,

				Text = "Who liek minecwaf?!"
			});

			_debugInfo = new GuiDebugInfo(alex);
			_debugInfo.AddDebugRight(() => $"Cursor Position: {alex.InputManager.CursorInputListener.GetCursorPosition()} / {alex.GuiManager.FocusManager.CursorPosition}");
			_debugInfo.AddDebugRight(() => $"Cursor Delta: {alex.InputManager.CursorInputListener.GetCursorPositionDelta()}");
			_debugInfo.AddDebugRight(() => $"Splash Text Scale: {_splashText.Scale:F3}");
			_debugInfo.AddDebugLeft(() => $"FPS: {FpsMonitor.Value:F0}");
		}

		private Texture2D _gradient;
		protected override void OnLoad(RenderArgs args)
		{
			using (MemoryStream ms = new MemoryStream(Resources.goodblur))
			{
				_gradient = Texture2D.FromStream(args.GraphicsDevice, ms);
			}
			//var logo = new UiElement()
			//{
			//	ClassName = "TitleScreenLogo",
			//};
			//Gui.AddChild(logo);

			//SynchronizationContext.Current.Send((o) => _backgroundSkyBox.Load(Alex.GuiRenderer), null);
			_splashText.Text = SplashTexts.GetSplashText();
			Alex.IsMouseVisible = true;
		}

		private float _rotation;

		protected override void OnUpdate(GameTime gameTime)
		{
			_backgroundSkyBox.Update(gameTime);

			_rotation += (float)gameTime.ElapsedGameTime.TotalMilliseconds / (1000.0f / 20.0f);

			_splashText.Scale = 0.65f + (float)Math.Abs(Math.Sin(MathHelper.ToRadians(_rotation * 10.0f))) * 0.5f;

			base.OnUpdate(gameTime);
		}

		protected override void OnDraw3D(RenderArgs args)
		{
			if (!_backgroundSkyBox.Loaded)
			{
				_backgroundSkyBox.Load(Alex.GuiRenderer);
			}

			_backgroundSkyBox.Draw(args);

			base.OnDraw3D(args);
			FpsMonitor.Update();
		}

		protected override void OnDraw2D(RenderArgs args)
		{
			args.SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
			args.SpriteBatch.Draw(_gradient, null, new Rectangle(0,0, Viewport.Width, Viewport.Height), null, null, 0f, null, new Color(Color.White, 0.5f), SpriteEffects.None);
			args.SpriteBatch.End();
			base.OnDraw2D(args);
		}

		protected override void OnShow()
		{
			base.OnShow();
			Alex.GuiManager.AddScreen(_debugInfo);
		}

		protected override void OnHide()
		{
			Alex.GuiManager.RemoveScreen(_debugInfo);
			base.OnHide();
		}

		private void Debug(IWorldGenerator generator)
		{
			Alex.IsMultiplayer = false;

			Alex.IsMouseVisible = false;

			generator.Initialize();
			var debugProvider = new SPWorldProvider(Alex, generator);
			Alex.LoadWorld(debugProvider, debugProvider.Network);
		}

		private void DebugFlatland()
		{
			Debug(new FlatlandGenerator());
		}

		private void DebugAnvil()
		{
			Debug(new AnvilWorldProvider(Alex.GameSettings.Anvil)
			{
				MissingChunkProvider = new VoidWorldGenerator()
			});
		}

		private void DebugWorldButtonActivated()
		{
			Debug(new DebugWorldGenerator());
		}
	}
}
