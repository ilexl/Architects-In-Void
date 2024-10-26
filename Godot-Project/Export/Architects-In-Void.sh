#!/bin/sh
echo -ne '\033c\033]0;Architects-In-Void\a'
base_path="$(dirname "$(realpath "$0")")"
"$base_path/Architects-In-Void.x86_64" "$@"
