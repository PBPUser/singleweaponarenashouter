shader_type spatial;
uniform vec3 damagePoint;
uniform mat3 basis;
uniform mat3 object_basis;
uniform float damageStrengh;

void vertex() {
	float damageThere = pow(max(0, damageStrengh-distance(damagePoint, VERTEX)), 2);
	vec3 direction = (basis) * vec3(.0, .0, 1.);
	VERTEX = VERTEX + (direction * damageThere);
}

void fragment() {
	vec4 c = COLOR;
	c.r = mix(c.r, 1.0f, 1.0f);
	c.g = mix(c.g, 0.0f, damageStrengh/2.0f);
	c.b = mix(c.b, 0.0f, damageStrengh/2.0f);
	ALBEDO = c.rgb;
	ALPHA = c.a;
}

//void light() {
//	// Called for every pixel for every light affecting the material.
//	// Uncomment to replace the default light processing function with this one.
//}
