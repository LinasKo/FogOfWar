package com.fog.structures;

import com.badlogic.gdx.maps.tiled.TiledMapTile;

/**
 * generic class for a building, storing its features.
 * 
 * @author Linasko
 */
public class Building {
	private TiledMapTile tile;
	private Structure type;
	private int player;
	private int health;
	private int x, y;
	private float sight_range;

	public Building(TiledMapTile tile, int x, int y, Structure type, int player, int health, float sight_range) {
		this.tile = tile;
		this.type = type;
		this.player = player;
		this.health = health;

		this.x = x;
		this.y = y;
		this.sight_range = sight_range;
	}

	public int getX() {
		return x;
	}

	public int getY() {
		return y;
	}

	public float getSight_range() {
		return sight_range;
	}
}
