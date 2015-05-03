package com.fog.structures;

import com.badlogic.gdx.maps.tiled.TiledMapTile;
	
public class Resource {
	
	public static int BASE_WOOD = 60;
	
	private TiledMapTile tile;
	private Structure type;
	private int amount;
	
	public Resource(TiledMapTile tile, Structure type, int amount) {
		this.tile = tile;
		this.type = type;
		this.amount = amount;
	}
}
