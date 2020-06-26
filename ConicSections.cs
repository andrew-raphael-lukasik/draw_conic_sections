#if DEBUG

using UnityEngine;
using Unity.Mathematics;

public static class ConicSections
{


	public static void DrawEllipse
	(
		float rx ,
		float ry ,
		Vector3 pos = default(Vector3) ,
		Quaternion rot = default(Quaternion) ,
		int numSegments = 128
	)
	{
		float theta = ( 2f * math.PI ) / (float)numSegments;
		for( int i=0 ; i<numSegments ; i++ )
		{
			float f0 = theta * (float)i;
			float f1 = theta * (float)(i+1);
			Vector3 v0 = rot * ( new Vector3{ x=math.cos(f0)*rx , y=math.sin(f0)*ry } );
			Vector3 v1 = rot * ( new Vector3{ x=math.cos(f1)*rx , y=math.sin(f1)*ry } );
			Gizmos.DrawLine( pos + v0 , pos + v1 );
		}

		float a = math.max(rx,ry);
		float b = math.min(rx,ry);
		float e = math.sqrt( 1f - (b*b)/(a*a) );
		float c = a * e;
		Vector3 foci = rx>ry ? new Vector3{x=c} : new Vector3{y=c};
		
		SetGizmosColorVariant();
		Gizmos.DrawWireSphere( pos + rot * foci , 0.05f );
		Gizmos.DrawWireSphere( pos + rot * -foci , 0.05f );
	}

	public static void DrawEllipseAtFoci
	(
		float rx ,
		float ry ,
		Vector3 pos = default(Vector3) ,
		Quaternion rot = default(Quaternion) ,
		int numSegments = 128
	)
	{
		float a = math.max(rx,ry);
		float b = math.min(rx,ry);
		float e = math.sqrt( 1f - (b*b)/(a*a) );
		float c = a * e;
		Vector3 foci = rx>ry ? new Vector3{x=c} : new Vector3{y=c};

		float theta = ( 2f * math.PI ) / (float)numSegments;
		for( int i=0 ; i<numSegments ; i++ )
		{
			float f0 = theta * (float)i;
			float f1 = theta * (float)(i+1);
			Vector3 v0 = rot * ( new Vector3{ x=math.cos(f0)*rx , y=math.sin(f0)*ry } + foci );
			Vector3 v1 = rot * ( new Vector3{ x=math.cos(f1)*rx , y=math.sin(f1)*ry } + foci );
			Gizmos.DrawLine( pos + v0 , pos + v1 );
		}
	}


	public static void DrawCircle
	(
		float r ,
		Vector3 pos = default(Vector3) ,
		Quaternion rot = default(Quaternion) ,
		int numSegments = 128
	)
	{
		float theta = ( 2f * math.PI ) / (float)numSegments;
		for( int i=0 ; i<numSegments ; i++ )
		{
			float f0 = theta * (float)i;
			float f1 = theta * (float)(i+1);
			Vector3 v0 = rot * ( new Vector3{ x=math.cos(f0) , y=math.sin(f0) } * r );
			Vector3 v1 = rot * ( new Vector3{ x=math.cos(f1) , y=math.sin(f1) } * r );
			Gizmos.DrawLine( pos + v0 , pos + v1 );
		}
	}


	/// <param name="a"> +-y = ( b * math.sqrt( **a**^2 + x^2 ) ) / **a** </param>
	/// <param name="b"> +-y = ( **b** * math.sqrt( a^2 + x^2 ) ) / a </param>
	public static void DrawHyperbolaAtFoci
	(
		float a , float b ,
		float xrange ,
		Vector3 pos = default(Vector3) ,
		Quaternion rot = default(Quaternion) ,
		int numSegments = 128
	)
	{
		float c = math.sqrt( a*a + b*b );
		Vector2 vertex = new Vector2{ y=a };
		Vector3 focus = new Vector2{ x=0 , y=c };
		float xmin = vertex.x - xrange;
		float xmax = vertex.x + xrange;
		for( int i=0 ; i<numSegments ; i++ )
		{
			float x0 = math.lerp( xmin , xmax , (float)i / (float)numSegments );
			float x1 = math.lerp( xmin , xmax , (float)(i+1) / (float)numSegments );
			Vector3 v0 = rot * ( new Vector3{ x=x0 , y=(b*math.sqrt(a*a+x0*x0))/a } - focus );
			Vector3 v1 = rot * ( new Vector3{ x=x1 , y=(b*math.sqrt(a*a+x1*x1))/a } - focus );
			Gizmos.DrawLine( pos + v0 , pos + v1 );
		}
		SetGizmosColorVariant();
		Gizmos.DrawWireSphere( pos , 0.05f );
	}

	/// <param name="a"> +-y = ( b * math.sqrt( **a**^2 + x^2 ) ) / **a** </param>
	/// <param name="b"> +-y = ( **b** * math.sqrt( a^2 + x^2 ) ) / a </param>
	public static void DrawHyperbola
	(
		float a , float b ,
		float xrange ,
		Vector3 pos = default(Vector3) ,
		Quaternion rot = default(Quaternion) ,
		int numSegments = 128
	)
	{
		float Asymptote ( float x ) => (b/a)*x;
		float c = math.sqrt( a*a + b*b );
		Vector2 vertex = new Vector2{ y=a };
		Vector3 focus = new Vector2{ y=c };
		
		Vector3 fmirror = new Vector3{ x=1 , y=-1f , z=1 };
		float xmin = vertex.x - xrange;
		float xmax = vertex.x + xrange;
		for( int i=0 ; i<numSegments ; i++ )
		{
			float x0 = math.lerp( xmin , xmax , (float)i / (float)numSegments );
			float x1 = math.lerp( xmin , xmax , (float)(i+1) / (float)numSegments );
			
			Vector3 p0 = new Vector3{ x=x0 , y=(b*math.sqrt(a*a+x0*x0))/a };
			Vector3 p1 = new Vector3{ x=x1 , y=(b*math.sqrt(a*a+x1*x1))/a };

			Vector3 v0 = rot * p0;
			Vector3 v1 = rot * p1;
			Gizmos.DrawLine( pos + v0 , pos + v1 );

			Vector3 v0b = rot * Vector3.Scale(p0,fmirror);
			Vector3 v1b = rot * Vector3.Scale(p1,fmirror);
			Gizmos.DrawLine( pos + v0b , pos + v1b );
		}
		SetGizmosColorVariant();
		Gizmos.DrawWireSphere( pos + rot * focus , 0.05f );
		Gizmos.DrawWireSphere( pos + rot * Vector3.Scale(focus,fmirror) , 0.05f );
		Gizmos.DrawLine(
			pos + ( rot * new Vector3{ x=xmin , y=Asymptote(xmin) } ) ,
			pos + ( rot * new Vector3{ x=xmax , y=Asymptote(xmax) } )
		);
		Gizmos.DrawLine(
			pos + ( rot * new Vector3{ x=xmin , y=-Asymptote(xmin) } ) ,
			pos + ( rot * new Vector3{ x=xmax , y=-Asymptote(xmax) } )
		);
	}



	/// <param name="a"> y = **a**xx + bx + c </param>
	/// <param name="b"> y = axx + **b**x + c </param>
	/// <param name="c"> y = axx + bx + **c** </param>
	public static void DrawParabola
	(
		float a , float b ,
		float xmin , float xmax ,
		Vector3 pos = default(Vector3) ,
		Quaternion rot = default(Quaternion) ,
		int numSegments = 128
	)
	{
		const float c = 0;
		Vector2 vertex = new Vector2{ x = -b / (2 * a) , y = ((4 * a * c) - (b * b)) / (4 * a) };
		Vector3 focus = new Vector2{ x = -b / (2 * a) , y = ((4 * a * c) - (b * b) + 1) / (4 * a) };
		float directrix_y = c - ((b*b) + 1) * 4 * a;

		for( int i=0 ; i<numSegments ; i++ )
		{
			float x0 = math.lerp( xmin , xmax , (float)i / (float)numSegments );
			float x1 = math.lerp( xmin , xmax , (float)(i+1) / (float)numSegments );
			Vector3 v0 = rot * new Vector3{ x=x0 , y=a*x0*x0+b*x0+c };
			Vector3 v1 = rot * new Vector3{ x=x1 , y=a*x1*x1+b*x1+c };
			Gizmos.DrawLine( pos + v0 , pos + v1 );
		}
		SetGizmosColorVariant();
		Gizmos.DrawLine(
			pos + rot * new Vector3{ x=xmin , y=directrix_y } ,
			pos + rot * new Vector3{ x=xmax , y=directrix_y }
		);
		Gizmos.DrawWireSphere( pos + rot * focus , 0.05f );
	}

	/// <param name="a"> y = **a**xx + bx + c </param>
	/// <param name="b"> y = axx + **b**x + c </param>
	/// <param name="c"> y = axx + bx + **c** </param>
	public static void DrawParabolaAtFoci
	(
		float a , float b ,
		float xrange ,
		Vector3 pos = default(Vector3) ,
		Quaternion rot = default(Quaternion) ,
		int numSegments = 128
	)
	{
		const float c = 0;
		Vector2 vertex = new Vector2{ x = -b / (2 * a) , y = ((4 * a * c) - (b * b)) / (4 * a) };
		Vector3 focus = new Vector2{ x = -b / (2 * a) , y = ((4 * a * c) - (b * b) + 1) / (4 * a) };
		float directrix_y = c - ((b*b) + 1) * 4 * a;

		float xmin = vertex.x - xrange;
		float xmax = vertex.x + xrange;
		for( int i=0 ; i<numSegments ; i++ )
		{
			float x0 = math.lerp( xmin , xmax , (float)i / (float)numSegments );
			float x1 = math.lerp( xmin , xmax , (float)(i+1) / (float)numSegments );
			Vector3 v0 = rot * ( new Vector3{ x=x0 , y=a*x0*x0+b*x0+c } - focus );
			Vector3 v1 = rot * ( new Vector3{ x=x1 , y=a*x1*x1+b*x1+c } - focus );
			Gizmos.DrawLine( pos + v0 , pos + v1 );
		}
		SetGizmosColorVariant();
		Gizmos.DrawLine(
			( rot * new Vector3{ x=xmin , y=directrix_y } - focus ) + new Vector3{x=pos.z} ,
			( rot * new Vector3{ x=xmax , y=directrix_y } - focus ) + new Vector3{x=pos.z}
		);
		Gizmos.DrawWireSphere( pos , 0.05f );
	}


	static void SetGizmosColorVariant () => Gizmos.color = Color.Lerp( Gizmos.color , new Color{ r=1f-Gizmos.color.r , g=1f-Gizmos.color.g , b=1f-Gizmos.color.b , a=Gizmos.color.a } , 0.33f );


}

#endif
