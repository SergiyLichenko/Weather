#pragma once
#include <vector>
#include "MyParticle.h"

using namespace std;
ref class Rain_ sealed:public MyParticle
{
	vector<D2D1_POINT_2F> point1;
	vector<D2D1_POINT_2F> point2;
	vector<int> speedX;
	vector<int> speedY;

	int width;
public:
	Rain_();

	int GetPoint1X(int i);
	int GetPoint1Y(int i);
	int GetPoint2X(int i);
	int GetPoint2Y(int i);
	int GetCountOfParticles() override;

	void Clear() override;
	void Move() override;
	void CreateParticles(int countOfPoints, int widthOfWindow);
};

