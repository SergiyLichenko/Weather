#pragma once
#include "pch.h"
#include "D2DPanel.h"
#include <windows.ui.xaml.media.dxinterop.h>


using namespace Microsoft::WRL;
using namespace Windows::UI::Core;
using namespace Windows::UI::Xaml;
using namespace DirectXPanels;
using namespace Platform;
using namespace DirectX;
using namespace D2D1;
using namespace DirectXPanels;

D2DPanel::D2DPanel() :
	loadingComplete(false),
	height(1.0f),
	width(1.0f),
	dpi(96.0f),
	currentType(ParticleType::Snow)
{
	CreateDevice();
	CreateDeviceResources();
	this->SizeChanged += ref new Windows::UI::Xaml::SizeChangedEventHandler(this, &D2DPanel::OnSizeChanged);
}

void D2DPanel::CreateParticles(int countOfPoints, int widthOfWindow)
{
	switch (currentType)
	{
	case ParticleType::Snow:
		snow.CreateParticles(countOfPoints);
		break;
	case ParticleType::Rain:
		rain.CreateParticles(countOfPoints, widthOfWindow);
	}
}
void D2DPanel::Clear(int newType)
{
	switch (newType)
	{
	case ParticleType::Snow:
		rain.Clear();
		this->currentType = ParticleType::Snow;
		break;

	case ParticleType::Rain:
		snow.Clear();
		this->currentType = ParticleType::Rain;
		break;
	}
	Render();
}
void D2DPanel::Move()
{
	switch (currentType)
	{
	case ParticleType::Snow:
		snow.Move();
		break;

	case ParticleType::Rain:
		rain.Move();
		break;	
	}
}


void D2DPanel::TrackPos(int posX, int posY)
{
	switch (currentType)
	{
	case ParticleType::Snow:
		snow.TrackPos(posX, posY);
		break;
	}
	Render();
}


void D2DPanel::Render()
{
	if (!loadingComplete)
	{
		return;
	}
	context2D->BeginDraw();
	context2D->Clear(ColorF(1.f, 1.f, 1.f, 0.0f));

	switch (currentType)
	{
	case ParticleType::Snow:
		for (int i = 0; i < snow.GetCountOfParticles(); i++)
		{
			D2D1_ELLIPSE ellipse;
			ellipse.point.x = snow.GetParticleX(i);
			ellipse.point.y = snow.GetParticleY(i);
			ellipse.radiusX = 1;
			ellipse.radiusY = 1;

			context2D->DrawEllipse(ellipse, solidBrush.Get(), 2);
		}
		break;
	case ParticleType::Rain:
		for (int i = 0; i < rain.GetCountOfParticles(); i++)
		{
			D2D1_POINT_2F point1;
			point1.x = rain.GetPoint1X(i);
			point1.y = rain.GetPoint1Y(i);

			D2D1_POINT_2F point2;
			point2.x = rain.GetPoint2X(i);
			point2.y = rain.GetPoint2Y(i);
			context2D->DrawLine(point1, point2, solidBrush.Get());
		}
		break;
	}

	context2D->EndDraw();

	DXGI_PRESENT_PARAMETERS parameters = { 0 };
	parameters.DirtyRectsCount = 0;

	swapChain->Present1(1, 0, &parameters);
}

void D2DPanel::OnSizeChanged(Object^ sender, SizeChangedEventArgs^ e)
{
	if (width != e->NewSize.Width || height != e->NewSize.Height)
	{
		width = max(e->NewSize.Width, 1.0f);
		height = max(e->NewSize.Height, 1.0f);

		CreateDeviceResources();
	}
}

void D2DPanel::CreateDevice()
{
	D2D1_FACTORY_OPTIONS options;
	ZeroMemory(&options, sizeof(D2D1_FACTORY_OPTIONS));

	D2D1CreateFactory(
		D2D1_FACTORY_TYPE_SINGLE_THREADED,
		__uuidof(ID2D1Factory2),
		&options,
		&factory2D
		);

	D3D_FEATURE_LEVEL featureLevels[] =
	{
		D3D_FEATURE_LEVEL_11_1,
		D3D_FEATURE_LEVEL_11_0,
	};

	ComPtr<ID3D11Device> tempDevice3D;
	ComPtr<ID3D11DeviceContext> tempContext3D;

	D3D11CreateDevice(
		nullptr,
		D3D_DRIVER_TYPE_HARDWARE,
		0,
		D3D11_CREATE_DEVICE_BGRA_SUPPORT,
		featureLevels,
		ARRAYSIZE(featureLevels),
		D3D11_SDK_VERSION,
		&tempDevice3D,
		NULL,
		&tempContext3D
		);

	tempDevice3D.As(&device3D);
	tempContext3D.As(&context3D);

	ComPtr<IDXGIDevice> dxgiDevice;
	device3D.As(&dxgiDevice);

	factory2D->CreateDevice(dxgiDevice.Get(), &device2D);

	device2D->CreateDeviceContext(
		D2D1_DEVICE_CONTEXT_OPTIONS_NONE,
		&context2D);

	context2D->CreateSolidColorBrush(ColorF(ColorF::White), &solidBrush);
	context2D->SetUnitMode(D2D1_UNIT_MODE::D2D1_UNIT_MODE_PIXELS);
	loadingComplete = true;
}

void D2DPanel::CreateDeviceResources()
{
	context2D->SetTarget(nullptr);
	targetBitmap2D = nullptr;
	context3D->OMSetRenderTargets(0, nullptr, nullptr);
	context3D->Flush();

	// If the swap chain already exists, then resize it.
	if (swapChain != nullptr)
	{
		HRESULT hr = swapChain->ResizeBuffers(
			2,
			static_cast<UINT>(width),
			static_cast<UINT>(height),
			DXGI_FORMAT_B8G8R8A8_UNORM,
			0
			);
	}
	else
	{
		DXGI_SWAP_CHAIN_DESC1 swapChainDesc = { 0 };
		swapChainDesc.Width = static_cast<UINT>(width);
		swapChainDesc.Height = static_cast<UINT>(height);
		swapChainDesc.Format = DXGI_FORMAT_B8G8R8A8_UNORM;
		swapChainDesc.Stereo = false;
		swapChainDesc.SampleDesc.Count = 1;
		swapChainDesc.SampleDesc.Quality = 0;
		swapChainDesc.BufferUsage = DXGI_USAGE_RENDER_TARGET_OUTPUT;
		swapChainDesc.BufferCount = 2;
		swapChainDesc.SwapEffect = DXGI_SWAP_EFFECT_FLIP_SEQUENTIAL;
		swapChainDesc.Flags = 0;
		swapChainDesc.AlphaMode = DXGI_ALPHA_MODE_PREMULTIPLIED;

		ComPtr<IDXGIDevice1> dxgiDevice;
		device3D.As(&dxgiDevice);

		ComPtr<IDXGIAdapter> dxgiAdapter;
		dxgiDevice->GetAdapter(&dxgiAdapter);

		ComPtr<IDXGIFactory2> dxgiFactory;
		dxgiAdapter->GetParent(IID_PPV_ARGS(&dxgiFactory));

		ComPtr<IDXGISwapChain1> tempSwapChain;
		dxgiFactory->CreateSwapChainForComposition(
			device3D.Get(),
			&swapChainDesc,
			nullptr,
			&tempSwapChain
			);

		tempSwapChain.As(&swapChain);
		dxgiDevice->SetMaximumFrameLatency(1);

		Dispatcher->RunAsync(CoreDispatcherPriority::Normal, ref new DispatchedHandler([=]()
		{
			ComPtr<ISwapChainPanelNative> panelNative;
			reinterpret_cast<IUnknown*>(this)->QueryInterface(IID_PPV_ARGS(&panelNative));
			panelNative->SetSwapChain(swapChain.Get());
		}, CallbackContext::Any));
	}
	D2D1_BITMAP_PROPERTIES1 bitmapProperties =
		BitmapProperties1(
			D2D1_BITMAP_OPTIONS_TARGET | D2D1_BITMAP_OPTIONS_CANNOT_DRAW,
			PixelFormat(DXGI_FORMAT_B8G8R8A8_UNORM, D2D1_ALPHA_MODE_PREMULTIPLIED),
			dpi,
			dpi);
	ComPtr<IDXGISurface> dxgiBackBuffer;

	swapChain->GetBuffer(0, IID_PPV_ARGS(&dxgiBackBuffer));
	context2D->CreateBitmapFromDxgiSurface(
		dxgiBackBuffer.Get(),
		&bitmapProperties,
		&targetBitmap2D
		);

	context2D->SetDpi(dpi, dpi);
	context2D->SetTarget(targetBitmap2D.Get());
}