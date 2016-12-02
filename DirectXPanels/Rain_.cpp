#include "pch.h"
#include "Rain_.h"


Rain_::Rain_()
{
	
}

int Rain_::GetPoint1X(int i)
{
	return point1.at(i).x;
}

int Rain_::GetPoint1Y(int i)
{
	return point1.at(i).y;
}

int Rain_::GetPoint2X(int i)
{
	return point2.at(i).x;
}

int Rain_::GetPoint2Y(int i)
{
	return point2.at(i).y;
}

int Rain_::GetCountOfParticles()
{
	return this->countOfPoints;
}

void Rain_::Clear()
{
	this->countOfPoints = 0;

	this->speedX.clear();
	this->speedY.clear();
	this->point1.clear();
	this->point2.clear();
}

void Rain_::Move()
{
	for (int i = 0; i < countOfPoints; i++)
	{
		point1.at(i).x += speedX.at(i);

		point1.at(i).y += speedY.at(i);

		point2.at(i).x += speedX.at(i);
		point2.at(i).y += speedY.at(i);
	};
}

void Rain_::CreateParticles(int countOfPoints,int widthOfWindow)
{
	this->countOfPoints += countOfPoints;
	for (int i = 0; i < countOfPoints; i++)
	{
		D2D1_POINT_2F first;
		first.x = rand() % widthOfWindow;
		first.y = 0;

		D2D1_POINT_2F second;
		if (rand() > RAND_MAX / 2)
		{
			second.x = first.x + rand() % 10;
			speedX.push_back(rand() % 10);
		}
		else
		{
			second.x = first.x - rand() % 10;
			speedX.push_back(-(rand() % 10));
		}
		second.y = rand() % 10;
		speedY.push_back(rand() % 40);

		point1.push_back(first);
		point2.push_back(second);
	};	
}
