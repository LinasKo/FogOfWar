package com.fog.units;

import com.badlogic.gdx.math.Vector2;

public class Unit {
	
	private static int baseHealth;
	private static int damage;
	private static float speed;
	
	private Vector2 position;
	private float direction;
	private int health;
	
	private boolean moving;
	
	/**
	 * Self explanatory:
	 * @param baseHealth
	 * @param damage
	 * @param maxSpeed
	 * @param position
	 * @param direction
	 */
	Unit(int baseHealth, int damage, int speed, Vector2 position, float direction) {
		this.setBaseHealth(baseHealth);
		this.setDamage(damage);
		this.setSpeed(speed);
		
		this.position = position;
		this.direction = direction;
		health = baseHealth;
	}

	public static int getDamage() {
		return damage;
	}

	public static void setDamage(int damage) {
		Unit.damage = damage;
	}

	public static float getSpeed() {
		return speed;
	}

	public static void setSpeed(float speed) {
		Unit.speed = speed;
	}

	public Vector2 getPosition() {
		return position;
	}

	public void setPosition(Vector2 position) {
		this.position = position;
	}

	public float getDirection() {
		return direction;
	}

	public void setDirection(float direction) {
		this.direction = direction;
	}

	public int getHealth() {
		return health;
	}

	public void setHealth(int health) {
		this.health = health;
	}

	public static int getBaseHealth() {
		return baseHealth;
	}

	public static void setBaseHealth(int baseHealth) {
		Unit.baseHealth = baseHealth;
	}
}
