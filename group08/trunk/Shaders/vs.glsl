#version 330
 
in vec3 a_Position;
in vec3 a_Normal;
in vec2 a_TexCoord;

uniform mat4 model;
uniform mat4 view;
uniform mat4 projection;

out vec3 v_FragPos;
out vec2 v_TexCoord;
out vec3 v_Normal;

void main()
{
	gl_Position = projection * view * model * vec4(a_Position, 1.0);
	v_FragPos = vec3(model * vec4(a_Position, 1.0));
	
	// Adjusting the normal for non uniform transformations.
	v_Normal = mat3(transpose(inverse(model))) * a_Normal;

	v_TexCoord = a_TexCoord;
}