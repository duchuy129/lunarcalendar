#!/bin/bash

# DigitalOcean Deployment Script
# This script helps you deploy the LunarCalendar API to DigitalOcean App Platform

set -e  # Exit on error

echo "=========================================="
echo "LunarCalendar API - DigitalOcean Deployment"
echo "=========================================="
echo ""

# Check if doctl is installed
if ! command -v doctl &> /dev/null; then
    echo "❌ Error: doctl (DigitalOcean CLI) is not installed"
    echo ""
    echo "Install it with:"
    echo "  macOS: brew install doctl"
    echo "  Linux: snap install doctl"
    echo "  Windows: Download from https://github.com/digitalocean/doctl/releases"
    echo ""
    exit 1
fi

echo "✅ doctl is installed"

# Check authentication
if ! doctl auth list &> /dev/null; then
    echo "⚠️  You need to authenticate with DigitalOcean"
    echo ""
    echo "Steps:"
    echo "1. Get your API token from: https://cloud.digitalocean.com/account/api/tokens"
    echo "2. Run: doctl auth init"
    echo "3. Paste your API token when prompted"
    echo ""
    exit 1
fi

echo "✅ doctl is authenticated"
echo ""

# Deployment options
echo "Choose deployment method:"
echo "1. Deploy from GitHub (Recommended)"
echo "2. Deploy from local Docker image"
echo ""
read -p "Enter choice (1 or 2): " choice

case $choice in
    1)
        echo ""
        echo "📦 Deploying from GitHub..."
        echo ""
        echo "Prerequisites:"
        echo "✓ Code pushed to GitHub"
        echo "✓ .do/app.yaml configured with your repo"
        echo "✓ GitHub access granted to DigitalOcean"
        echo ""
        read -p "Continue? (y/n): " confirm

        if [ "$confirm" != "y" ]; then
            echo "Deployment cancelled"
            exit 0
        fi

        # Create or update app
        if [ -f ".do/app.yaml" ]; then
            echo "Creating app from app.yaml..."
            doctl apps create --spec .do/app.yaml
            echo ""
            echo "✅ App created successfully!"
            echo ""
            echo "Next steps:"
            echo "1. Go to: https://cloud.digitalocean.com/apps"
            echo "2. Find your app and configure secrets:"
            echo "   - JwtSettings__SecretKey (generate with: openssl rand -base64 32)"
            echo "3. Your app will auto-deploy when ready"
        else
            echo "❌ Error: .do/app.yaml not found"
            exit 1
        fi
        ;;

    2)
        echo ""
        echo "🐳 Deploying from local Docker image..."
        echo ""

        # Build Docker image
        read -p "Enter app name (e.g., lunarcalendar-api): " APP_NAME

        echo "Building Docker image..."
        docker build -t $APP_NAME .

        echo ""
        echo "⚠️  Note: DigitalOcean App Platform uses GitHub/GitLab for container builds"
        echo "For local images, consider using DigitalOcean Container Registry (DOCR):"
        echo ""
        echo "1. Create registry: doctl registry create lunarcalendar"
        echo "2. Login: doctl registry login"
        echo "3. Tag image: docker tag $APP_NAME registry.digitalocean.com/lunarcalendar/$APP_NAME"
        echo "4. Push: docker push registry.digitalocean.com/lunarcalendar/$APP_NAME"
        echo "5. Update .do/app.yaml to use the registry image"
        echo ""
        ;;

    *)
        echo "Invalid choice"
        exit 1
        ;;
esac

echo ""
echo "=========================================="
echo "🎉 Deployment initiated!"
echo "=========================================="
echo ""
echo "Monitor your deployment:"
echo "  Dashboard: https://cloud.digitalocean.com/apps"
echo "  CLI: doctl apps list"
echo ""
echo "View logs:"
echo "  doctl apps logs <app-id> --type run"
echo ""
echo "Useful commands:"
echo "  List apps: doctl apps list"
echo "  Get app info: doctl apps get <app-id>"
echo "  Update app: doctl apps update <app-id> --spec .do/app.yaml"
echo "  Delete app: doctl apps delete <app-id>"
echo ""
