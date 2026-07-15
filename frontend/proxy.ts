import { NextResponse } from "next/server";
import type { NextRequest } from "next/server";

const PROTECTED_PATHS = [
  "/dashboard",
  "/dashboard/qr",
  "/dashboard/reviews",
  "/dashboard/invoices",
  "/dashboard/customers",
  "/dashboard/analytics",
  "/dashboard/whatsapp",
  "/dashboard/ai-assistant",
  "/dashboard/settings",
];

const AUTH_PATHS = ["/login", "/register", "/forgot-password"];

export function proxy(request: NextRequest) {
  const { pathname } = request.nextUrl;

  const token = request.cookies.get("businessos_token")?.value;

  const isProtected = PROTECTED_PATHS.some(
    (path) => pathname === path || pathname.startsWith(path + "/")
  );
  const isAuthPage = AUTH_PATHS.some((path) => pathname.startsWith(path));

  if (isProtected && !token) {
    const loginUrl = new URL("/login", request.url);
    loginUrl.searchParams.set("from", pathname);
    return NextResponse.redirect(loginUrl);
  }

  if (isAuthPage && token) {
    return NextResponse.redirect(new URL("/dashboard", request.url));
  }

  return NextResponse.next();
}

export const config = {
  matcher: [
    "/((?!_next/static|_next/image|favicon.ico|api/).*)",
  ],
};
