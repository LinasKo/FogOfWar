package com.fog.structures;

import com.badlogic.gdx.maps.tiled.TiledMapTile;

public class Building {
	private TiledMapTile tile;
	private Structure type;
	private int player;
	private int health;
	
	public Building(TiledMapTile tile, Structure type, int player, int health) {
		this.tile = tile;
		this.type = type;
		this.player = player;
		this.health = health;
	}
}
