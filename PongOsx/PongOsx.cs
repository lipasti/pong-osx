using System;
using System.Collections.Generic;
using Jypeli;
using Jypeli.Assets;
using Jypeli.Controls;
using Jypeli.Effects;
using Jypeli.Widgets;

public class PongOsx : PhysicsGame
{
	Vector nopeusYlos = new Vector(0, 500);
	Vector nopeusAlas = new Vector(0, -500);

	PhysicsObject pallo;
	PhysicsObject maila1;
	PhysicsObject maila2;
	PhysicsObject vasenReuna;
	PhysicsObject oikeaReuna;
    PhysicsObject keskipallo;

	IntMeter pelaajan1pisteet;
	IntMeter pelaajan2pisteet;
	public override void Begin()
	{
		LuoKentta();
		maila1 = LuoMaila(Level.Left + 20.0);
		maila2 = LuoMaila(Level.Right - 20.0);
		LisaaLaskurit();
		AloitaPeli();
		AsetaOhjaimet();


	}

	void LuoKentta()
	{
		pallo = new PhysicsObject(40.0, 40.0);
		pallo.Shape = Shape.Circle;
		pallo.Restitution = 1.0;
        pallo.Color = Color.Red;
		Add(pallo);
		vasenReuna = Level.CreateLeftBorder();
		vasenReuna.Restitution = 1.0;
		vasenReuna.IsVisible = false;

		oikeaReuna = Level.CreateRightBorder();
		oikeaReuna.Restitution = 1.0;
		oikeaReuna.IsVisible = false;

		PhysicsObject ylaReuna = Level.CreateTopBorder();
		ylaReuna.Restitution = 1.0;
		ylaReuna.IsVisible = false;

		PhysicsObject alaReuna = Level.CreateBottomBorder();
		alaReuna.Restitution = 1.0;
		alaReuna.IsVisible = false;

		keskipallo = new PhysicsObject(200.0, 200.0);
		keskipallo.Shape = Shape.Circle;
        keskipallo.Color = Color.Gray;
        keskipallo.Mass = 10;
        keskipallo.Restitution = 10.0;
        Add(keskipallo);

		Level.Background.Color = Color.Black;
		Camera.ZoomToLevel();
		AddCollisionHandler(pallo, KasittelePallonTormays);
	}

	void KasittelePallonTormays(PhysicsObject pallo, PhysicsObject kohde)
	{
		if (kohde == oikeaReuna)
		{
			pelaajan1pisteet.Value += 1;
		}
		else if (kohde == vasenReuna)
		{
			pelaajan2pisteet.Value += 1;
		}
	}

	PhysicsObject LuoMaila(double x)
	{
		PhysicsObject maila = PhysicsObject.CreateStaticObject(20.0, 100.0);
		maila.Shape = Shape.Rectangle;
		maila.X = x;
		maila.Y = 0.0;
		maila.Restitution = 1.0;
		Add(maila);
		return maila;
	}

	void AsetaOhjaimet()
	{
		Keyboard.Listen(Key.Escape, ButtonState.Pressed, ConfirmExit, "Lopeta peli");
		Keyboard.Listen(Key.F1, ButtonState.Pressed, ShowControlHelp, "Näytä ohjeet");

		Keyboard.Listen(Key.A, ButtonState.Down, AsetaNopeus, "Pelaaja 1: Liikuta mailaa ylös", maila1, nopeusYlos);
		Keyboard.Listen(Key.A, ButtonState.Released, AsetaNopeus, null, maila1, Vector.Zero);
		Keyboard.Listen(Key.Z, ButtonState.Down, AsetaNopeus, "Pelaaja 1: Liikuta mailaa alas", maila1, nopeusAlas);
		Keyboard.Listen(Key.Z, ButtonState.Released, AsetaNopeus, null, maila1, Vector.Zero);

		Keyboard.Listen(Key.Up, ButtonState.Down, AsetaNopeus, "Pelaaja 2: Liikuta mailaa ylös", maila2, nopeusYlos);
		Keyboard.Listen(Key.Up, ButtonState.Released, AsetaNopeus, null, maila2, Vector.Zero);
		Keyboard.Listen(Key.Down, ButtonState.Down, AsetaNopeus, "Pelaaja 2: Liikuta mailaa alas", maila2, nopeusAlas);
		Keyboard.Listen(Key.Down, ButtonState.Released, AsetaNopeus, null, maila2, Vector.Zero);


	}

	void AsetaNopeus(PhysicsObject maila, Vector nopeus)
	{


		if ((nopeus.Y > 0) && (maila.Top > Level.Top))
		{
			maila.Velocity = Vector.Zero;
			return;
		}

		if ((nopeus.Y < 0) && (maila.Bottom < Level.Bottom))
		{
			maila.Velocity = Vector.Zero;
			return;
		}
		maila.Velocity = nopeus;

	}

	void AloitaPeli()
	{
		Vector impulssi = new Vector(500.0, 0.0);
		pallo.Hit(impulssi);
	}
	void LisaaLaskurit()
	{
		pelaajan1pisteet = LuoPisteLaskuri(Screen.Left + 100.0, Screen.Top - 100.0);
		pelaajan2pisteet = LuoPisteLaskuri(Screen.Right - 100.0, Screen.Top - 100.0);
	}
	IntMeter LuoPisteLaskuri(double X, double Y)
	{
		IntMeter laskuri = new IntMeter(0);
		laskuri.MaxValue = 10;
		laskuri.AddTrigger(1, TriggerDirection.Irrelevant, GameOver);

		Label naytto = new Label();
		naytto.BindTo(laskuri);
		naytto.X = X;
		naytto.Y = Y;
		naytto.TextColor = Color.Aqua;
		naytto.BorderColor = Level.Background.Color;
		naytto.Color = Level.Background.Color;
		Add(naytto);

		return laskuri;
	}

	void GameOver()
	{
        StopAll();
        Label gameOver = new Label(
            600.0,
            200.0,
            "Game over!"
        );
        gameOver.TextColor = Color.White;
        Add(gameOver);
        Keyboard.Clear();
        Keyboard.Listen(Key.Enter, ButtonState.Pressed, SuljePeli, "Lopeta");
	}

    void SuljePeli() {
        Exit();
    }
}
