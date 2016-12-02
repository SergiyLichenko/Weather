#include "pch.h"
#include "Snow_.h"


Snow_::Snow_()
{
}

void Snow_::CreateParticles(int countOfPoints)
{
	this->countOfPoints += countOfPoints;
	for (int i = 0; i < countOfPoints; i++)
	{
		D2D1_ELLIPSE tempParticle;
		tempParticle.point.x = 0;
		tempParticle.point.y = 0;
		tempParticle.radiusX = 1;
		tempParticle.radiusY = 1;
		particles.push_back(tempParticle);

		int* pos = new int[2]{ 0 };
		this->deltaPos.push_back(pos);
	}
}

void Snow_::Clear()
{
	this->countOfPoints = 0;
	this->deltaPos.clear();
	this->particles.clear();
}

void Snow_::Move()
{
	for (int i = 0; i < countOfPoints; i++)
	{
		if (rand() > RAND_MAX / 2)
			deltaPos.at(i)[0] += rand() % 10;
		else
			deltaPos.at(i)[0] -= rand() % 10;
		deltaPos.at(i)[1] += rand() % 10;
	}
}

void Snow_::TrackPos(int posX, int posY)
{
	if (posX != -1 && posY != -1)
	{
		this->currentX = posX;
		this->currentY = posY;
	}
	for (int i = 0;i < countOfPoints; i++)
	{
		particles.at(i).point.x = currentX + this->deltaPos[i][0];
		particles.at(i).point.y = currentY + this->deltaPos[i][1];
	}
}

int Snow_::GetParticleX(int i)
{
	return this->particles.at(i).point.x;
}

int Snow_::GetParticleY(int i)
{
	return this->particles.at(i).point.y;
}

int Snow_::GetCountOfParticles()
{
	return this->countOfPoints;
}
