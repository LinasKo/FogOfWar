package com.fog.structures;

import com.badlogic.gdx.maps.tiled.TiledMapTile;

/**
 * Generic class for a natural resource, storing its features.
 * 
 * @author Linasko
 */
public abstract class Resource {

	//public static int BASE_WOOD = 60;

	private TiledMapTile tile;
	//private Structure type;
	private int amount;
	private int x, y;
	
	public Resource(TiledMapTile tile, int x, int y, int amount) {
		this.tile = tile;
		//this.type = type;
		this.amount = amount;
		this.x = x;
		this.y = y;
	}
	
	public void setAmount(int val){
		amount = val;
	}
	
	public int getAmount(){
		return amount;
	}
}
