#pragma once
#include <vector>
#include "MyParticle.h"

using namespace std;
ref class Snow_ sealed:public MyParticle
{
	vector<D2D1_ELLIPSE> particles;
	vector<int*> deltaPos;//offset from cursor
	int currentX;
	int currentY;

public:
	Snow_();

	int GetParticleX(int i);
	int GetParticleY(int i);
	int GetCountOfParticles() override;

	
	void Clear() override;
	void Move() override;
	void TrackPos(int posX, int posY);
	void CreateParticles(int countOfPoints);
	
};

