#pragma once
#include "pch.h"
#include <vector>
#include <string>
#include "Rain_.h"
#include "Snow_.h"
using namespace std;
using namespace Microsoft::WRL;
namespace DirectXPanels
{
	enum ParticleType
	{
		Snow,
		Rain
	};
	public ref class D2DPanel sealed : public Windows::UI::Xaml::Controls::SwapChainPanel
	{
		ParticleType currentType;		
		Rain_ rain;
		Snow_ snow;


		float dpi;
		float height;
		float width;
		bool loadingComplete;

		ComPtr<ID2D1Device> device2D;
		ComPtr<ID2D1Factory2> factory2D;
		ComPtr<ID3D11Device1> device3D;
		ComPtr<ID3D11DeviceContext1> context3D;
		ComPtr<IDXGISwapChain2> swapChain;
		ComPtr<ID2D1DeviceContext> context2D;
		ComPtr<ID2D1Bitmap1> targetBitmap2D;

		ComPtr<ID2D1SolidColorBrush> solidBrush;
	public:
		D2DPanel();

		void Move();
		void TrackPos(int posX, int poxY);
		void CreateParticles(int count,int widthOfWindow);
		void Clear(int newType);
	private:
		void OnSizeChanged(Platform::Object^ sender, Windows::UI::Xaml::SizeChangedEventArgs^ e);

		void CreateDevice();
		void CreateDeviceResources();
		void Render();
	};


	class RainP
	{
	public:
		int speedX;
		int speedY;
		D2D1_POINT_2F point1;
		D2D1_POINT_2F point2;
	};
}
