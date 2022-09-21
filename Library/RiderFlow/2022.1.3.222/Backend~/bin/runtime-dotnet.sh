#!/bin/sh

set -e

case $(uname) in
Darwin) platform=macos
    case $(uname -m) in
    x86_64) architecture=x64;;
    arm64)  architecture=arm64;;
    *) echo "Unknown architecture: $(uname -m)" >&2; exit 2;;
    esac;;
Linux) platform=$({ ldd --version 2>&1 || true; } | grep -q musl && echo linux-musl || echo linux)
    case $(linux$(getconf LONG_BIT) uname -m) in
    x86_64)          architecture=x64;;
    aarch64)         architecture=arm64;;
    armv7l | armv8l) architecture=arm;;
    *) echo "Unknown architecture: $(linux$(getconf LONG_BIT) uname -m)" >&2; exit 2;;
    esac;;
*) echo "Unknown platform: $(uname)" >&2; exit 1;;
esac

root=$(cd "$(dirname "$0")"; pwd)
dotnet="$root/$platform-$architecture/dotnet"

# Unblock files when downloaded in Safari
if [ "x$platform" = "xmacos" ] && xattr "$dotnet" | grep -q com.apple.quarantine; then
  xattr -d -r com.apple.quarantine "$dotnet"
fi

exec "$dotnet/dotnet" "$@"
