#pragma once
ref class MyParticle
{

internal:
	int countOfPoints;

	virtual int GetCountOfParticles() { return 0; };
public:
	virtual void Clear() { ; };
	virtual void Move() { ; };
};

